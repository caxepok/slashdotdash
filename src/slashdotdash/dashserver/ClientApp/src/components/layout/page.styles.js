import styled from "styled-components";
import { NavLink } from "react-router-dom";

export const Wrapper = styled.div`
  border-radius: ${(p) => p.theme.borderRadius.card};
  background: #fff;
  padding: ${(p) => p.theme.spacing.xlarge * 2}px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  align-items: stretch;
  gap: ${(p) => p.theme.spacing.xlarge};
  box-shadow: ${(p) => p.theme.shadows.card};
  text-decoration: none;
  color: ${(p) => p.theme.colors.secondary};
  transition: box-shadow 0.1s, transform 0.1s;
  position: relative;
`;

export const Title = styled.h2`
  text-align: center;
  ${(p) => p.theme.typography.title.h2};
  color: ${(p) => p.theme.colors.primary};
`;

export const Content = styled.div`
  flex-basis: 100%;
  display: flex;
`;

export const BackButton = styled(NavLink)`
  width: 32px;
  height: 32px;
  fill: ${(p) => p.theme.colors.accent};
  position: absolute;
  left: ${(p) => p.theme.spacing.xlarge};
  top: ${(p) => p.theme.spacing.xlarge};
`;
