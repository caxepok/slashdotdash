import React, { useCallback } from "react";
import { AutoSizer } from "react-virtualized";
import { HorizontalBarSeries, LineSeries, VerticalBarSeries, XYPlot } from "react-vis";
import { useTheme } from "styled-components";
import format from "date-fns/format";
import { useChartLine } from "./chart.hooks";
import * as Markup from "./chart.styles";

export const ChartBar = React.memo((props) => {
  const { colors } = useTheme();
  const [data, threshold, limits] = useChartLine(props);

  return (
    <AutoSizer>
      {(size) => (
        <Markup.Chart>
          <XYPlot {...size} margin={{ left: 0, top: 0, right: 0, bottom: 0 }}>
            <LineSeries data={limits} color={"transparent"} />
            <LineSeries data={threshold} color={"#FF0000"} style={{ strokeWidth: 1, strokeDasharray: 2 }} />
            <VerticalBarSeries data={data} color={colors.accent} />
          </XYPlot>
        </Markup.Chart>
      )}
    </AutoSizer>
  );
});
