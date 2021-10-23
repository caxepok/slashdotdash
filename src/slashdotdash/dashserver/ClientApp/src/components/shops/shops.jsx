import React, { useEffect } from "react";
import { shallowEqual, useDispatch, useSelector } from "react-redux";
import { loadShopsData } from "../../reducers/dashboard";
import { ChartBar } from "../chart";

export const Shops = React.memo((props) => {
  const shopData = useSelector(({ dashboard }) => dashboard.shopData, shallowEqual);
  const date = useSelector(({ dashboard }) => dashboard.date.shop);
  const dispatch = useDispatch();
  useEffect(() => dispatch(loadShopsData(date)), [dispatch, date]);

  return <ChartBar {...props} values={shopData} />;
});
