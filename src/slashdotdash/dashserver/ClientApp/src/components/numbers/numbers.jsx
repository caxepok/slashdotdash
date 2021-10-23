import React from "react";
import * as Markup from "./numbers.styles";

const Numbers = React.memo((props) => {
  const { values } = props;

  return (
    <Markup.Wrapper>
      {values.map((item, key) => (
        <Markup.Number type={item.type} key={key}>
          {item.value}
        </Markup.Number>
      ))}
    </Markup.Wrapper>
  );
});
