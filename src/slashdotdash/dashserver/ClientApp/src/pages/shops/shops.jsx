import React, { useEffect } from "react";
import { shallowEqual, useDispatch, useSelector } from "react-redux";
import { loadShopData } from "../../reducers/dashboard";
import { ChartBar, DatePicker, Layout } from "../../components";
import * as Markup from "./shops.styles";

export const Shops = React.memo((props) => {
  const shopData = useSelector(({ dashboard }) => dashboard.shopData, shallowEqual);
  const date = useSelector(({ dashboard }) => dashboard.date);
  const dispatch = useDispatch();
  useEffect(() => dispatch(loadShopData(date)), [dispatch, date]);

  return (
    <Layout.Card
      title={
        <>
          Загрузка оборудования на
          <Markup.DatePickerWrapper>
            <DatePicker />
          </Markup.DatePickerWrapper>
        </>
      }>
      <Layout.Row sizes={[1, 14, 1]}>
        <span />
        <Layout.Column sizes={[1, "auto"]}>
          <ChartBar {...props} values={shopData} />
          <p>Какой-то текст</p>
        </Layout.Column>
        <span />
      </Layout.Row>
    </Layout.Card>
  );
});
