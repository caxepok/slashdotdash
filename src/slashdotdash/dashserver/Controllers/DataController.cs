using CsvHelper;
using CsvHelper.Configuration;
using dashserver.Enums;
using dashserver.Infrastructure;
using dashserver.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace dashserver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly DashDBContext _dashDBContext;

        public DataController(ILogger<DataController> logger, DashDBContext dashDBContext)
        {
            _logger = logger;
            _dashDBContext = dashDBContext;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            _logger.LogInformation("ping");
            return Ok("pong");
        }

        /// <summary>
        /// Загрузка производственного плана
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload/plan")]
        public async Task<IActionResult> Upload([FromQuery]DateTimeOffset planDate, [FromBody]byte[] data)
        {
            Stream stream = new MemoryStream(data);
            using var reader = new StreamReader(stream);
            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };
            using var csv = new CsvReader(reader, csvConfiguration);

            List<Shop> shops = _dashDBContext.Shops.ToList();
            List<ResourceGroup> resourceGroups = _dashDBContext.ResourceGroups.ToList();
            List<Resource> resources = _dashDBContext.Resources.ToList();
            List<Plan> plans = _dashDBContext.Plans.ToList();
            List<PlanDay> days = new();

            while (csv.Read())
            {
                //csv[0]  // номер просто
                //csv[1]  // тоже номер просто
                string shopName = csv[2];
                string resourceGroupCode = csv[3];
                string resourceGroupName = csv[4];
                string resourceCode = csv[5];
                string resourceName = csv[6];
                string dStart = csv[7];
                string day = csv[8];
                string durationDays = csv[9];
                string occupiedPercent = csv[10];
                string unavailablePercentage = csv[11];

                DateTimeOffset startDate = DateTimeOffset.ParseExact(dStart, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                Shop shop = shops.SingleOrDefault(_ => _.Name == shopName);
                if (shop == null)
                {
                    shop = new Shop(shopName);
                    shops.Add(shop);
                    _dashDBContext.Add(shop);
                    await _dashDBContext.SaveChangesAsync();
                }

                ResourceGroup rg = resourceGroups.SingleOrDefault(_ => _.Code == resourceGroupCode);
                if (rg == null)
                {
                    rg = new ResourceGroup(resourceGroupCode, resourceGroupName, shop.Id);
                    resourceGroups.Add(rg);
                    _dashDBContext.Add(rg);
                    await _dashDBContext.SaveChangesAsync();
                }

                Resource resource = resources.SingleOrDefault(_ => _.Code == resourceCode);
                if(resource == null)
                {
                    resource = new Resource(resourceCode, resourceName, rg.Id);
                    resources.Add(resource);
                    _dashDBContext.Add(resource);
                    await _dashDBContext.SaveChangesAsync();
                }

                Plan plan = plans.SingleOrDefault(_ => _.Date == planDate);
                if(plan == null)
                {
                    plan = new Plan(planDate);
                    plans.Add(plan);
                    _dashDBContext.Add(plan);
                    await _dashDBContext.SaveChangesAsync();
                }

                PlanDay planDay = new PlanDay(plan.Id, resource.Id, rg.Id, startDate, Int32.Parse(day), Int32.Parse(durationDays), Decimal.Parse(occupiedPercent), Decimal.Parse(unavailablePercentage));
                days.Add(planDay);
            }

            _dashDBContext.AddRange(days);
            await _dashDBContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Загрузка связок складов и групп агрегатов
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload/stock/links")]
        public async Task<IActionResult> UploadStock([FromBody] byte[] data)
        {
            Stream stream = new MemoryStream(data);
            using var reader = new StreamReader(stream);
            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };
            using var csv = new CsvReader(reader, csvConfiguration);

            List<Stock> stocks = _dashDBContext.Stocks.ToList();
            List<ResourceGroup> resourceGroups = _dashDBContext.ResourceGroups.ToList();
            List<StockLink> stockLinks = _dashDBContext.StockLinks.ToList();

            while (csv.Read())
            {
                // csv[0];
                // csv[1];
                // csv[2];
                string resourceGroupCode = csv[3];
                string[] inStockCodes = csv[4].Trim('\"').Split(';', StringSplitOptions.RemoveEmptyEntries);
                string[] outStockCodes = csv[5].Trim('\"').Split(';', StringSplitOptions.RemoveEmptyEntries);

                ResourceGroup rg = resourceGroups.SingleOrDefault(_ => _.Code == resourceGroupCode);
                if (rg == null)
                {
                    _logger.LogWarning($"Resource group not found, skipped, code: {resourceGroupCode}");
                    continue;
                }

                foreach (var stockCode in inStockCodes)
                {
                    Stock stock = stocks.SingleOrDefault(_ => _.Code == stockCode);
                    if (stock == null)
                    {
                        _logger.LogWarning($"In stock not found, stockCode: {stockCode}, resourceCode: {resourceGroupCode}");
                        continue;
                    }

                    StockLink stockLink = _dashDBContext.StockLinks.SingleOrDefault(_ => _.StockId == stock.Id && _.ResourceGroupId == rg.Id && _.Type == StockLinkType.In);
                    if (stockLink == null)
                    {
                        _dashDBContext.StockLinks.Add(new StockLink(rg.Id, stock.Id, StockLinkType.In));
                    }
                }

                foreach (var stockCode in outStockCodes)
                {
                    Stock stock = stocks.SingleOrDefault(_ => _.Code == stockCode);
                    if (stock == null)
                    {
                        _logger.LogWarning($"Out stock not found, stockCode: {stockCode}, resourceCode: {resourceGroupCode}");
                        continue;
                    }

                    StockLink stockLink = _dashDBContext.StockLinks.SingleOrDefault(_ => _.StockId == stock.Id && _.ResourceGroupId == rg.Id && _.Type == StockLinkType.Out);
                    if (stockLink == null)
                    {
                        _dashDBContext.StockLinks.Add(new StockLink(rg.Id, stock.Id, StockLinkType.Out));
                    }
                }
            }

            _dashDBContext.AddRange(stockLinks);
            await _dashDBContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Загрузка кладских запасов
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload/stock/balances")]
        public async Task<IActionResult> UploadStock([FromQuery] DateTimeOffset planDate, [FromBody] byte[] data)
        {
            Stream stream = new MemoryStream(data);
            using var reader = new StreamReader(stream);
            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };
            using var csv = new CsvReader(reader, csvConfiguration);

            Plan plan = _dashDBContext.Plans.SingleOrDefault(_ => _.Date == planDate);
            if (plan == null)
                return BadRequest($"No plan found for stock balances upload at date {planDate}");

            List<Stock> stocks = _dashDBContext.Stocks.ToList();
            List<StockBalance> stockBalances = _dashDBContext.StockBalances.ToList();

            while (csv.Read())
            {
                string stockCode = csv[0];
                string stockName = csv[1];
                // csv[2];
                string date = csv[3];
                string balance = csv[4];
                string allowedOverload = csv[5];
                string maxBalance = csv[6];

                DateTimeOffset stockDate = DateTimeOffset.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                Stock stock= stocks.SingleOrDefault(_ => _.Code == stockCode);
                if (stock == null)
                {
                    stock = new Stock(stockCode, stockName);
                    stocks.Add(stock);
                    _dashDBContext.Add(stock);
                    await _dashDBContext.SaveChangesAsync();
                }
                stockBalances.Add(new StockBalance(plan.Id, stock.Id, stockDate, Decimal.Parse(balance), Decimal.Parse(maxBalance), Boolean.Parse(allowedOverload)));
            }

            _dashDBContext.AddRange(stockBalances);
            await _dashDBContext.SaveChangesAsync();

            return Ok();
        }
    }
}
