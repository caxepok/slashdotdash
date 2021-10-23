import styled from "styled-components";

export const Wrapper = styled.span`
  ${(p) => p.theme.typography.title.h3};
  color: ${(p) => p.theme.colors.accent};
  display: flex;
  flex-basis: 100%;
  justify-content: center;
  align-items: center;
  cursor: pointer;
`;
