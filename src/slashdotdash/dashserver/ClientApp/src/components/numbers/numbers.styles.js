import styled from "styled-components";
import { colors } from "../../consts";

export const Wrapper = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
`;

export const Number = styled.span`
  ${(p) => p.theme.typography.title.h3};
  color: ${(p) => colors[p.type]};

  &:before {
    content: "/";
    display: inline-block;
    margin: 0 0.5em;
    color: ${(p) => p.theme.colors.tertiary};
  }

  &:first-child:before {
    display: none;
  }
`;
