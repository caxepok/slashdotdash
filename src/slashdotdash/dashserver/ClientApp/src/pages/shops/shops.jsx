import React, { useEffect } from "react";
import { shallowEqual, useDispatch, useSelector } from "react-redux";
import { loadData, loadPlanData, loadShopData } from "../../reducers/dashboard";
import { ChartBar, DatePicker, Layout } from "../../components";
import * as Markup from "./shops.styles";
import { chartTypes } from "../../consts";

export const Shops = React.memo(() => {
  const shopData = useSelector(({ dashboard }) => dashboard.shopData, shallowEqual);
  const chartData = useSelector(
    ({ dashboard }) =>
      dashboard.data && dashboard.data.find((item) => item.type === chartTypes.planning.resourceWorkload),
    shallowEqual,
  );
  const date = useSelector(({ dashboard }) => dashboard.date);
  const dispatch = useDispatch();
  useEffect(() => dispatch(loadShopData(date)), [dispatch, date]);
  useEffect(() => dispatch(loadPlanData(date)), [dispatch, date]);
  useEffect(() => dispatch(loadData()), [dispatch]);

  return (
    <Layout.Page
      backTo={"/"}
      title={
        <>
          Загрузка оборудования на
          <Markup.DatePickerWrapper>
            <DatePicker />
          </Markup.DatePickerWrapper>
        </>
      }>
      <ChartBar {...chartData} values={shopData} />
    </Layout.Page>
  );
});
