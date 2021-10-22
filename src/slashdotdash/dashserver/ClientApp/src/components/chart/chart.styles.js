import styled from "styled-components";

export const Chart = styled.span`
  display: block;
  margin-left: -15px;
  margin-bottom: -20px;

  svg {
    text {
      font-size: 10px;
      fill: #999;
    }

    .rv-xy-plot__series--line {
      fill: none;
      stroke-width: 2px;
    }

    .rv-xy-plot__axis__line {
      stroke: #0000001f;
    }

    .rv-xy-plot__grid-lines {
      stroke: #0000001f;
    }
  }
`;
