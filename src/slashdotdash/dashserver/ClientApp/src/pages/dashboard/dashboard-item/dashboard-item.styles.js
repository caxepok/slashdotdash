import styled from "styled-components";

export const Wrapper = styled.div`
  display: flex;
  flex-direction: column;
  align-items: stretch;
  flex-basis: 100%;
  gap: ${(p) => p.theme.spacing.medium};
  text-decoration: none;
`;

export const Title = styled.h4`
  ${(p) => p.theme.typography.title.h4};
  font-weight: 400;
  color: ${(p) => p.theme.colors.primary};
  text-align: center;
  margin: 0;
`;

export const Content = styled.div`
  flex-basis: 100%;
  align-items: stretch;
  display: flex;
  flex-direction: ${(p) => (p.horizontal ? "row" : "column")};
  gap: ${(p) => p.theme.spacing.medium};
`;
