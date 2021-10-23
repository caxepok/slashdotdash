import React, { useMemo } from "react";
import { PlanTableRow } from "./plan-table-row";
import * as Markup from "./plan-table.styles";
import { AutoSizer } from "react-virtualized";
import format from "date-fns/format";

export const PlanTable = React.memo((props) => {
  const { data, threshold } = props;
  const headData = useMemo(() => {
    if (!data || !data.length) return null;

    return data[0].values.map((x) => x.date);
  }, [data]);

  if (!data) {
    return null;
  }

  return (
    <AutoSizer>
      {(style) => (
        <Markup.Table style={style}>
          <Markup.Head>
            <span />
            {headData.map((item, index) => (
              <Markup.Value key={index}>{format(new Date(item), "dd/MM")}</Markup.Value>
            ))}
          </Markup.Head>
          <Markup.Body>
            {data.map((item) => (
              <PlanTableRow key={`${item.name}${item.id}`} {...item} threshold={threshold} />
            ))}
          </Markup.Body>
        </Markup.Table>
      )}
    </AutoSizer>
  );
});
