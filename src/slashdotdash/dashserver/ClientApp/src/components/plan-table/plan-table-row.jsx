import React, { useMemo, useState } from "react";
import { v4 } from "uuid";
import ReactTooltip from "react-tooltip";
import { ReactComponent as ExpandIcon } from "../../assets/shevron.svg";
import * as Markup from "./plan-table.styles";
import { colors } from "../../consts";

const getCellStyles = (value, threshold) => {
  if (value < threshold - 5 || value > 100)
    return { background: `${colors.danger}`, color: "#FFFFFF", fontWeight: 700 };
  if (value > threshold + 5) return { background: `${colors.success}44` };
  return { background: `${colors.warning}` };
};

export const PlanTableRow = React.memo((props) => {
  const { name, childs, values, threshold, depth = 0 } = props;
  const [isExpanded, setExpanded] = useState(false);
  const hasChildren = useMemo(() => Boolean(childs && childs.length), [childs]);
  const onClick = useMemo(() => {
    return hasChildren ? () => setExpanded((value) => !value) : undefined;
  }, [hasChildren]);

  return (
    <>
      <Markup.Row>
        <Markup.Title depth={depth} onClick={onClick}>
          {hasChildren && (
            <Markup.Shevron isExpanded={isExpanded}>
              <ExpandIcon />
            </Markup.Shevron>
          )}
          {name}
        </Markup.Title>
        {values.map((item, index) => {
          const id = `tooltip-${v4()}`;
          if (item.warning) {
            console.log(item);
          }
          return (
            <Markup.Value
              key={index}
              data-tip
              data-for={id}
              {...getCellStyles(item.value, threshold)}
              hasWarning={item.warning}>
              {item.value.toFixed(0)}%
              {item.warning && (
                <ReactTooltip id={id} aria-haspopup="true">
                  {item.warning}
                </ReactTooltip>
              )}
            </Markup.Value>
          );
        })}
      </Markup.Row>
      {hasChildren &&
        isExpanded &&
        childs.map((item) => (
          <PlanTableRow depth={depth + 1} key={`${item.name}${item.id}`} {...item} threshold={threshold} />
        ))}
    </>
  );
});
