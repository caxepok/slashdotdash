import React from "react";
import { AutoSizer } from "react-virtualized";
import { LineSeries, XYPlot } from "react-vis";
import { useTheme } from "styled-components";
import { useChartLine } from "./chart.hooks";
import { ChartHorizontalGrid } from "./chart-horizontal-grid";
import * as Markup from "./chart.styles";

export const ChartLine = React.memo((props) => {
  const { values, threshold } = props;
  const { colors } = useTheme();
  const [data, limits] = useChartLine(values);

  return (
    <Markup.Wrapper>
      <Markup.Chart threshold={threshold}>
        <AutoSizer>
          {(size) => (
            <XYPlot {...size} margin={{ left: 0, top: 0, right: 0, bottom: 0 }}>
              <ChartHorizontalGrid />
              <LineSeries data={limits} color={"transparent"} />
              <LineSeries data={data} color={colors.accent} />
            </XYPlot>
          )}
        </AutoSizer>
      </Markup.Chart>
    </Markup.Wrapper>
  );
});
