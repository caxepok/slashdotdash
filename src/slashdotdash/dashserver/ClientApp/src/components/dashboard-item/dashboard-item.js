import React, { useMemo } from "react";
import { shallowEqual, useSelector } from "react-redux";
import { Layout } from "../layout";

export const DashboardItem = React.memo((props) => {
  const { type, as: Component } = props;
  const data = useSelector(({ dashboard }) => dashboard.data && dashboard.data[type], shallowEqual);

  const chartData = useMemo(() => {
    if (!data || !data.values) return [];
    return data.values
      .map((item) => ({ x: new Date(item.date).getTime(), y: item.value }))
      .sort((a, b) => (a.x > b.x ? 1 : -1));
  }, [data]);

  if (!data) {
    return <Layout.Card>Loading</Layout.Card>;
  }

  return (
    <Layout.Card title={data.kpi.name}>
      <Component meta={data.kpi} data={chartData} />
    </Layout.Card>
  );
});
