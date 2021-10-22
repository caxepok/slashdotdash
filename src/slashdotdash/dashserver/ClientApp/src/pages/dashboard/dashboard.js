import React, { useEffect } from "react";
import { Layout } from "../../components";
import { Chart } from "../../components/chart";
import { useDispatch } from "react-redux";
import { fetchData } from "../../reducers/dashboard";
import { DashboardItem } from "../../components/dashboard-item";

export const Dashboard = React.memo(() => {
  const dispatch = useDispatch();
  useEffect(() => dispatch(fetchData()), [dispatch]);

  return (
    <Layout.Column sizes={[1, 2, 1]}>
      <Layout.Row count={4}>
        <DashboardItem type={0} as={Chart} />
        <DashboardItem type={1} as={Chart} />
        <DashboardItem type={2} as={Chart} />
        <DashboardItem type={3} as={Chart} />
      </Layout.Row>
      <Layout.Row sizes={[1, 2, 1]}>
        <Layout.Column count={3}>
          <Layout.Row count={2}>
            <DashboardItem type={4} as={Chart} />
            <DashboardItem type={5} as={Chart} />
          </Layout.Row>
          <DashboardItem type={6} as={Chart} />
          <DashboardItem type={7} as={Chart} />
        </Layout.Column>
        <DashboardItem type={8} as={Chart} />
        <Layout.Column count={2}>
          <DashboardItem type={9} as={Chart} />
          <DashboardItem type={10} as={Chart} />
        </Layout.Column>
      </Layout.Row>

      <Layout.Row count={5}>
        <DashboardItem type={11} as={Chart} />
        <DashboardItem type={12} as={Chart} />
        <DashboardItem type={13} as={Chart} />
        <DashboardItem type={14} as={Chart} />
        <DashboardItem type={15} as={Chart} />
      </Layout.Row>
    </Layout.Column>
  );
});
