import React, { useEffect } from "react";
import { shallowEqual, useDispatch, useSelector } from "react-redux";
import { loadShopsData, setDate } from "../../reducers/dashboard";
import { ChartBar, DatePicker, Layout } from "../../components";
import { ShopsItem } from "./shops-item";
import * as Markup from "./shops.styles";

export const Shops = React.memo(() => {
  const shopData = useSelector(({ dashboard }) => dashboard.shopData, shallowEqual);
  const date = useSelector(({ dashboard }) => dashboard.date.shop);
  const dispatch = useDispatch();
  useEffect(() => dispatch(loadShopsData(date)), [dispatch, date]);

  return (
    <Layout.Page
      backTo={"/"}
      title={
        <>
          Загрузка оборудования на
          <Markup.DatePickerWrapper>
            <DatePicker value={date} onChange={(value) => dispatch(setDate(value, "shop"))} />
          </Markup.DatePickerWrapper>
        </>
      }>
      <Layout.Column sizes={[4, 2]}>
        <ChartBar values={shopData} />
        <Layout.Row sizes={[3, 3, 3, 3, 5, 5]}>
          {shopData &&
            shopData.map((item) => <ShopsItem key={item.shopId} title={item.name} id={item.shopId} date={date} />)}
        </Layout.Row>
      </Layout.Column>
    </Layout.Page>
  );
});
