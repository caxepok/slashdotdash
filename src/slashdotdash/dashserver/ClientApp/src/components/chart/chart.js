import React, { useCallback, useMemo } from "react";
import { AutoSizer } from "react-virtualized";
import { LineSeries, XAxis, XYPlot, YAxis, HorizontalGridLines } from "react-vis";
import { useTheme } from "styled-components";
import format from "date-fns/format";
import * as Markup from "./chart.styles";

export const Chart = React.memo((props) => {
  const { data, meta } = props;
  const { colors } = useTheme();

  const tickFormat = useCallback((value) => format(value, "dd/MM"), []);
  const tresholdData = useMemo(() => {
    if (!data || !meta || meta.threshold === undefined) return [];
    return [
      { x: data[0].x, y: meta.threshold },
      { x: data[data.length - 1].x, y: meta.threshold },
    ];
  }, [meta, data]);

  const sizeData = useMemo(
    () =>
      data
        ? [
            { x: data[0].x, y: 0 },
            { x: data[data.length - 1].x, y: 0 },
          ]
        : [],
    [data],
  );

  return (
    <AutoSizer>
      {({ width, height }) => (
        <Markup.Chart>
          <XYPlot width={width + 15} height={height + 20}>
            <YAxis />
            <XAxis tickFormat={tickFormat} />
            <HorizontalGridLines />
            <LineSeries data={sizeData} color={"transparent"} />
            <LineSeries data={data} color={colors.accent} />
            <LineSeries data={tresholdData} color={"#FF0000"} style={{ strokeWidth: 1, strokeDasharray: 2 }} />
          </XYPlot>
        </Markup.Chart>
      )}
    </AutoSizer>
  );
});
