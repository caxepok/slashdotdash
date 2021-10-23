import React, { useEffect } from "react";
import { Layout, ChartLine, ChartRound, Shops } from "../../components";
import { useDispatch } from "react-redux";
import { loadData } from "../../reducers/dashboard";
import { DashboardItem } from "./dashboard-item";
import { chartTypes } from "../../consts";

export const DashboardOld = React.memo(() => {
  const dispatch = useDispatch();
  useEffect(() => dispatch(loadData()), [dispatch]);

  return (
    <Layout.Column sizes={[5, 2, 3]}>
      <Layout.Row sizes={[2, 3, 4]}>
        <Layout.Card>
          <Layout.Column sizes={["auto", 1]}>
            <DashboardItem type={chartTypes.orderFailures} />
            <DashboardItem type={chartTypes.quotaFill} as={ChartLine} />
          </Layout.Column>
        </Layout.Card>
        <Layout.Card>
          <Layout.Row count={2}>
            <Layout.Column sizes={[1, "auto"]}>
              <DashboardItem type={chartTypes.planning.plannedOTIF} as={ChartLine} />
              <DashboardItem type={chartTypes.planning.companyLoads} />
            </Layout.Column>
            <Layout.Column sizes={["auto", 1]}>
              <DashboardItem type={chartTypes.planning.storageFailures} />
              <DashboardItem type={chartTypes.planning.hot} as={ChartLine} />
            </Layout.Column>
          </Layout.Row>
        </Layout.Card>
        <Layout.Card>
          <DashboardItem type={chartTypes.combinations} as={Shops} />
        </Layout.Card>
      </Layout.Row>
      <Layout.Card>
        <Layout.Row count={3}>
          <DashboardItem horizontal type={chartTypes.daily.tasks} as={ChartRound} />
          <DashboardItem type={chartTypes.daily.tasksReserves} as={ChartLine} />
          <DashboardItem type={chartTypes.daily.tasksContourSystem} as={ChartLine} />
        </Layout.Row>
      </Layout.Card>
      <Layout.Row sizes={[2, 5, 2]}>
        <Layout.Card>1</Layout.Card>
        <Layout.Card>2</Layout.Card>
        <Layout.Card>3</Layout.Card>
      </Layout.Row>
    </Layout.Column>
  );
});
