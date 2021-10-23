import React, { useEffect } from "react";
import { shallowEqual, useDispatch, useSelector } from "react-redux";
import { loadComparePlanData, loadPlanData, setDate } from "../../reducers/dashboard";
import { DatePicker, Layout, PlanTable } from "../../components";
import * as Markup from "../shops/shops.styles";
import { PlanCompare } from "./plan-compare";

export const Plan = React.memo(() => {
  const planData = useSelector(({ dashboard }) => dashboard.planData, shallowEqual);
  const comparePlanData = useSelector(({ dashboard }) => dashboard.comparePlanData, shallowEqual);
  const date = useSelector(({ dashboard }) => dashboard.date.plan);
  const compareDate = useSelector(({ dashboard }) => dashboard.date.planCompare);

  const dispatch = useDispatch();
  useEffect(() => {
    if (!compareDate) {
      dispatch(loadPlanData(date));
    } else {
      dispatch(loadComparePlanData(date, compareDate));
    }
  }, [dispatch, date, compareDate]);

  return (
    <Layout.Page
      backTo={"/"}
      title={
        <>
          Планирование на
          <Markup.DatePickerWrapper>
            <DatePicker value={date} onChange={(value) => dispatch(setDate(value, "plan"))} />
          </Markup.DatePickerWrapper>
          <PlanCompare />
        </>
      }>
      <PlanTable data={compareDate ? comparePlanData : planData} threshold={80} isCompare={Boolean(compareDate)} />
    </Layout.Page>
  );
});
