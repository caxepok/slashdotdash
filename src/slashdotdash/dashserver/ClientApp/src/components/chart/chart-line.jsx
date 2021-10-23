import React, { useCallback, useMemo } from "react";
import { AutoSizer } from "react-virtualized";
import { LineSeries, XAxis, XYPlot, YAxis, HorizontalGridLines, VerticalGridLines } from "react-vis";
import { useTheme } from "styled-components";
import * as Markup from "./chart.styles";
import { useChartLine } from "./chart.hooks";

export const ChartLine = React.memo((props) => {
  const { values, threshold } = props;
  const { colors } = useTheme();
  const [data, limits] = useChartLine(values);

  return (
    <Markup.Chart threshold={threshold}>
      <AutoSizer>
        {(size) => (
          <XYPlot {...size} margin={{ left: 0, top: 0, right: 0, bottom: 0 }}>
            <HorizontalGridLines />
            <VerticalGridLines />
            <LineSeries data={limits} color={"transparent"} />
            <LineSeries data={data} color={colors.accent} />
          </XYPlot>
        )}
      </AutoSizer>
    </Markup.Chart>
  );
});
