import React, { useEffect } from "react";
import { Layout } from "../../components";
import { useDispatch } from "react-redux";
import { loadData } from "../../reducers/dashboard";
import { chartTypes } from "../../consts";
import { DashboardSummary } from "./dashboard-summary";
import { NavLink } from "react-router-dom";

export const Dashboard = React.memo(() => {
  const dispatch = useDispatch();
  useEffect(() => dispatch(loadData()), [dispatch]);

  return (
    <Layout.Column count={3}>
      <Layout.Row sizes={[1, 1, 1, 2]}>
        <Layout.Card title="Прием заказов">
          <DashboardSummary type={chartTypes.orderFailures} />
        </Layout.Card>
        <Layout.Card title="Управление квотами">
          <DashboardSummary type={chartTypes.quotaFill} />
        </Layout.Card>
        <Layout.Card title="Комбинирование заказов">
          <DashboardSummary type={chartTypes.combinations} />
        </Layout.Card>
        <Layout.Card title="Графикование конвертеров">
          <Layout.Row count={2}>
            <DashboardSummary type={chartTypes.converter.series} />
            <DashboardSummary type={chartTypes.converter.planFollowing} />
          </Layout.Row>
        </Layout.Card>
      </Layout.Row>
      <Layout.Card title={<NavLink to="/plan">Календарное планирование</NavLink>}>
        <DashboardSummary type={chartTypes.planning.resourceWorkload} to={"/shops"} />
        <DashboardSummary type={chartTypes.planning.plannedOTIF} />
        <DashboardSummary type={chartTypes.planning.storageFailures} />
        <DashboardSummary type={chartTypes.planning.companyLoads} />
        <DashboardSummary type={chartTypes.planning.hot} />
      </Layout.Card>
      <Layout.Row count={2}>
        <Layout.Card title="Графикование горячих цехов">
          <Layout.Row count={3}>
            <DashboardSummary type={chartTypes.hotShop.montage} />
            <DashboardSummary type={chartTypes.hotShop.planFollowing} />
            <DashboardSummary type={chartTypes.hotShop.reserve} />
          </Layout.Row>
        </Layout.Card>
        <Layout.Card title="Составление сменно-суточных заданий">
          <Layout.Row count={3}>
            <DashboardSummary type={chartTypes.daily.tasks} />
            <DashboardSummary type={chartTypes.daily.tasksReserves} />
            <DashboardSummary type={chartTypes.daily.tasksContourSystem} />
          </Layout.Row>
        </Layout.Card>
      </Layout.Row>
    </Layout.Column>
  );
});
