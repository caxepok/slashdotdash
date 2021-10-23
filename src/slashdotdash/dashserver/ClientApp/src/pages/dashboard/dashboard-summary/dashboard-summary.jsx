import React, { useRef } from "react";
import { shallowEqual, useSelector } from "react-redux";
import { ChartLine, ChartRound, Popup } from "../../../components";
import * as Markup from "../dashboard-item/dashboard-item.styles";
import { NavLink } from "react-router-dom";

export const DashboardSummary = (props) => {
  const { type, to } = props;
  const data = useSelector(
    ({ dashboard }) => dashboard.data && dashboard.data.find((item) => item.type === type),
    shallowEqual,
  );
  const triggerRef = useRef(null);

  if (!data) {
    return null;
  }

  return (
    <Markup.Wrapper as={to ? NavLink : undefined} to={to}>
      <Markup.Content ref={triggerRef}>
        <ChartRound value={data.todayValue} color={data.color} />
        <Popup title={data.name} triggerRef={triggerRef} clickable={!to}>
          <Markup.Popup>
            <ChartLine {...data} />
          </Markup.Popup>
        </Popup>
      </Markup.Content>
      <Markup.Title>{data.name}</Markup.Title>
    </Markup.Wrapper>
  );
};
