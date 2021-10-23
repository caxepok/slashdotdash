import styled, { css } from "styled-components";

export const Chart = styled.span`
  display: block;
  flex-basis: 100%;
  width: 100%;
  margin: 10px 5px;

  ${(p) =>
    p.threshold &&
    css`
      &:before {
        position: absolute;
        content: "";
        left: 0;
        right: 0;
        bottom: 0;
        height: ${p.threshold}%;
        border-top: 1px dashed #cc0000aa;
      }
    `}

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
