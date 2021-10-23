import React, { useEffect } from "react";
import { shallowEqual, useDispatch, useSelector } from "react-redux";
import { loadData, loadShopData } from "../../reducers/dashboard";
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
  useEffect(() => dispatch(loadData()), [dispatch]);

  return (
    <Layout.Page
      backTo={"/dashboard"}
      title={
        <>
          Загрузка оборудования на
          <Markup.DatePickerWrapper>
            <DatePicker />
          </Markup.DatePickerWrapper>
        </>
      }>
      <Layout.Column sizes={[1, "auto"]}>
        <ChartBar {...chartData} values={shopData} />
        <p>Какой-то текст</p>
      </Layout.Column>
    </Layout.Page>
  );
});
