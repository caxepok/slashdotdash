import React, { useEffect } from "react";
import { Layout } from "../../components";
import { ChartLine } from "../../components/chart";
import { DatePicker } from "../../components/date-picker";
import { useDispatch } from "react-redux";
import { fetchData } from "../../reducers/dashboard";
import { DashboardItem } from "../../components/dashboard-item";
import { chartTypes } from "../../consts";

export const Dashboard = React.memo(() => {
  const dispatch = useDispatch();
  useEffect(() => dispatch(fetchData()), [dispatch]);

  return (
    <Layout.Column sizes={[5, 2, 3]}>
      <Layout.Row sizes={[2, 3, 4]}>
        <Layout.Column sizes={["53px", 1]}>
          <Layout.Card>
            <DatePicker />
          </Layout.Card>
          <Layout.Card>
            <Layout.Column count={2}>
              <DashboardItem type={chartTypes.orderFailures} as={ChartLine} />
              <DashboardItem type={chartTypes.quotaFill} as={ChartLine} />
            </Layout.Column>
          </Layout.Card>
        </Layout.Column>
        <Layout.Card>
          <Layout.Row count={2}>
            <Layout.Column sizes={[2, 1]}>
              <DashboardItem type={chartTypes.planning.plannedOTIF} as={ChartLine} />
              <DashboardItem type={chartTypes.planning.companyLoads} as={ChartLine} />
            </Layout.Column>
            <Layout.Column sizes={[1, 2]}>
              <DashboardItem type={chartTypes.planning.storageFailures} as={ChartLine} />
              <DashboardItem type={chartTypes.planning.hot} as={ChartLine} />
            </Layout.Column>
          </Layout.Row>
        </Layout.Card>
        <Layout.Card>
          <DashboardItem type={chartTypes.combinations} as={ChartLine} />
        </Layout.Card>
      </Layout.Row>
    </Layout.Column>
  );
});
