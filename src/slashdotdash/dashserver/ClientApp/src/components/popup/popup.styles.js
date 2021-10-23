import styled from "styled-components";

export const Wrapper = styled.div`
  position: fixed;
  border-radius: ${(p) => p.theme.borderRadius.card};
  box-shadow: ${(p) => p.theme.shadows.cardHover}, 0 0 0 1px ${(p) => p.theme.colors.quarternary}66;
  padding: ${(p) => p.theme.spacing.xlarge};
  gap: ${(p) => p.theme.spacing.large};
  display: flex;
  flex-direction: column;
  align-items: stretch;
  background: #ffffff;
  z-index: ${(p) => (p.isFixed ? 101 : 100)};
  opacity: 0;
  transition: opacity 0.1s;
  user-select: none;

  &:focus {
    z-index: 102;
  }
`;

export const Header = styled.div`
  display: flex;
  ${(p) => p.theme.typography.title.h4};
  max-width: 100%;
  padding-right: 40px;
  align-items: center;
  width: 400px;
  height: 40px;
  margin-top: -10px;
`;

export const Button = styled.div`
  position: absolute;
  right: 10px;
  top: 10px;
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
  fill: ${(p) => (p.isFixed ? "#FFFFFF" : p.theme.colors.accent)};
  background: ${(p) => (p.isFixed ? p.theme.colors.accent : `${p.theme.colors.quarternary}44`)};
  border-radius: 3px;
  cursor: pointer;

  &:hover {
    fill: ${(p) => (p.isFixed ? "#FFFFFF" : p.theme.colors.accent)};
    background: ${(p) => (p.isFixed ? p.theme.colors.accent : `${p.theme.colors.quarternary}66`)};
  }

  > svg {
    width: 24px;
    height: 24px;
  }
`;
