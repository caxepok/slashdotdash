import styled from "styled-components";

export const Wrapper = styled.div`
  display: flex;
  flex-direction: column;
  align-items: stretch;
  flex-basis: 100%;
`;

export const Title = styled.h4`
  ${(p) => p.theme.typography.title.h4};
  color: ${(p) => p.theme.colors.secondary};
  text-align: center;
  margin: 0;
`;

export const Content = styled.div`
  flex-basis: 100%;
  align-items: stretch;
  display: flex;
`;
