import React, { useMemo } from "react";
import { shallowEqual, useSelector } from "react-redux";
import { ChartRound } from "../../../components";
import * as Markup from "../dashboard-item/dashboard-item.styles";
import { colors } from "../../../consts";
import { NavLink } from "react-router-dom";

export const DashboardSummary = (props) => {
  const { type, to } = props;
  const data = useSelector(
    ({ dashboard }) => dashboard.data && dashboard.data.find((item) => item.type === type),
    shallowEqual,
  );

  const color = useMemo(() => {
    if (!data) return null;
    const { todayValue, threshold } = data;
    return todayValue >= threshold ? colors["success"] : colors["danger"];
  }, [data]);

  if (!data) {
    return null;
  }

  return (
    <Markup.Wrapper as={to ? NavLink : undefined} to={to}>
      <Markup.Content>
        <ChartRound value={data.todayValue} color={color} />
      </Markup.Content>
      <Markup.Title>{data.name}</Markup.Title>
    </Markup.Wrapper>
  );
};
