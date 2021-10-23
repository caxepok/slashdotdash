import React, { useMemo } from "react";
import { shallowEqual, useSelector } from "react-redux";
import { Layout } from "../layout";
import * as Markup from "./dashboard-item.styles";

export const DashboardItem = React.memo((props) => {
  const { type, as: Component } = props;
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
      <Markup.Content>
        <Component {...data} />
      </Markup.Content>
    </Markup.Wrapper>
  );
});
