import styled from "styled-components";

export const Wrapper = styled.div`
  border-radius: ${(p) => p.theme.borderRadius.card};
  background: #fff;
  padding: ${(p) => p.theme.spacing.xlarge};
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  align-items: stretch;
  gap: ${(p) => p.theme.spacing.large};
  box-shadow: ${(p) => p.theme.shadows.card};
  text-decoration: none;
  color: ${(p) => p.theme.colors.secondary};
  transition: box-shadow 0.1s, transform 0.1s;

  a&:hover {
    box-shadow: ${(p) => p.theme.shadows.cardHover};
    transform: translateY(-1px);
  }

  a&:active {
    box-shadow: ${(p) => p.theme.shadows.cardActive};
    transform: none;
  }
`;

export const Title = styled.h3`
  margin: -2px 0 0 0;
  ${(p) => p.theme.typography.title.h3};
  color: ${(p) => p.theme.colors.primary};
`;

export const Content = styled.div`
  flex-basis: 100%;
`;
