import React, { useEffect } from "react";
import { shallowEqual, useDispatch, useSelector } from "react-redux";
import { loadPlanData } from "../../reducers/dashboard";
import { Layout, PlanTable } from "../../components";

export const Plan = React.memo(() => {
  const planData = useSelector(({ dashboard }) => dashboard.planData, shallowEqual);
  const date = useSelector(({ dashboard }) => dashboard.date);
  const dispatch = useDispatch();
  useEffect(() => dispatch(loadPlanData(date)), [dispatch, date]);

  return (
    <Layout.Page backTo={"/"} title="Планирование">
      <PlanTable data={planData} threshold={80} />
    </Layout.Page>
  );
});
