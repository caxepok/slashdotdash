import React, { useMemo } from "react";
import * as Markup from "./chart.styles";
import { ChartHorizontalGrid } from "./chart-horizontal-grid";

export const ChartBar = React.memo((props) => {
  const { values, threshold } = props;
  const legends = useMemo(() => (values ? values.map((item) => item.name) : []), [values]);
  return (
    <Markup.Wrapper>
      <Markup.Chart threshold={threshold}>
        <ChartHorizontalGrid />
        <Markup.Bars>
          {values &&
            values.map((item, key) => (
              <Markup.Bar key={key} value={item.value} type={item.value >= threshold ? "success" : "danger"}>
                <span>{item.value.toFixed(1)}%</span>
              </Markup.Bar>
            ))}
        </Markup.Bars>
      </Markup.Chart>

      <Markup.Legend>
        {legends.map((item) => (
          <span key={item}>{item}</span>
        ))}
      </Markup.Legend>
    </Markup.Wrapper>
  );
});
