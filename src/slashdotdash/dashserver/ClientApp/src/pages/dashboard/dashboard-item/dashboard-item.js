import React from "react";
import { shallowEqual, useSelector } from "react-redux";
import * as Markup from "./dashboard-item.styles";
import { Numbers } from "../../../components";

export const DashboardItem = React.memo((props) => {
  const { type, as: Component, horizontal } = props;
  const data = useSelector(
    ({ dashboard }) => dashboard.data && dashboard.data.find((item) => item.type === type),
    shallowEqual,
  );

  if (!data) {
    return data === null ? "Ошибка загрузки" : "Загрузка";
  }

  return (
    <Markup.Wrapper>
      <Markup.Title>{data.name}</Markup.Title>
      <Markup.Content horizontal={horizontal}>
        {Component && <Component {...data} />}
        <Numbers {...data} />
      </Markup.Content>
    </Markup.Wrapper>
  );
});
