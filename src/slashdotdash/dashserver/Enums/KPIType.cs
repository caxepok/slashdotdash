namespace dashserver.Enums
{
    /// <summary>
    /// Тип показателя КПЭ
    /// </summary>
    public enum KPIType
    {
        /// <summary>
        /// Заказы не прошедшие проверку на техническую исполнимость
        /// </summary>
        FailedOrders = 0,
        /// <summary>
        /// % заполнения квот
        /// </summary>
        QuotaFill = 1,
        /// <summary>
        /// Загрузка оборудования
        /// </summary>
        ResourceWorkload = 2,
        /// <summary>
        /// Плановый OTIF
        /// </summary>
        PlannedOTIF = 3,
        /// <summary>
        /// Нарушение уровней запасов
        /// </summary>
        StorageFailures = 4,
        /// <summary>
        /// Загрузка кампаний
        /// </summary>
        CompanyLoads = 5,
        /// <summary>
        /// Обязательный горячий посад, %
        /// </summary>
        Hot = 6,
        /// <summary>
        /// Формирование и передача комбинаций
        /// </summary>
        Combinations = 7,
        /// <summary>
        /// Составление и передача серий
        /// </summary>
        ConverterSeies = 8,
        /// <summary>
        /// Следование календарному плану
        /// </summary>
        ConverterPlanFollowing = 9,
        /// <summary>
        /// Составление и передача монтажей
        /// </summary>
        HotShopMontage = 10,
        /// <summary>
        /// Следование календарному плану
        /// </summary>
        HotShopPlanFollowing = 11,
        /// <summary>
        /// Уровень резервирования
        /// </summary>
        HotShopReserve = 12,
        /// <summary>
        /// % составления ССЗ
        /// </summary>
        DailyTasks = 13,
        /// <summary>
        /// Уровень резервирования
        /// </summary>
        DailyTasksReserves = 14,
        /// <summary>
        /// % составления заданий через контур системы
        /// </summary>
        DailyTasksContourSystem = 15
    }
}
