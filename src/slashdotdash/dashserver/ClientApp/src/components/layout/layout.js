import React from "react";
import * as Markup from "./card.styles";
import styled from "styled-components";
import { NavLink } from "react-router-dom";

const Card = React.memo(
  React.forwardRef((props, ref) => {
    const { title, children, placeholder } = props;

    return (
      <Markup.Wrapper ref={ref}>
        {title && <Markup.Title>{title}</Markup.Title>}
        <Markup.Content>{children}</Markup.Content>
        {placeholder && <Markup.Placeholder>{placeholder}</Markup.Placeholder>}
      </Markup.Wrapper>
    );
  }),
);

const NavCard = React.memo(
  React.forwardRef((props, ref) => {
    const { title, children, ...restProps } = props;

    return (
      <Markup.Wrapper as={NavLink} ref={ref} {...restProps}>
        {title && <Markup.Title>{title}</Markup.Title>}
        <Markup.Content>{children}</Markup.Content>
      </Markup.Wrapper>
    );
  }),
);

const Row = styled.div`
  display: grid;
  gap: ${(p) => p.theme.spacing.xlarge};
  grid-template-columns: ${(p) =>
    (p.sizes ? p.sizes.map((s) => `${s}fr`) : [...new Array(p.count || 1)].map((_) => "1fr")).join(" ")};
`;

const Column = styled.div`
  display: grid;
  grid-template-rows: ${(p) =>
    (p.sizes ? p.sizes.map((s) => `${s}fr`) : [...new Array(p.count || 1)].map((_) => "1fr")).join(" ")};
  gap: ${(p) => p.theme.spacing.xlarge};
`;

const Layout = styled.div`
  display: flex;
  align-items: stretch;
  width: 100%;
  min-height: 100%;
  position: absolute;
  padding: ${(p) => p.theme.spacing.xlarge};
  background: ${(p) => p.theme.colors.background};

  > * {
    flex-basis: 100%;
  }
`;

Layout.Card = Card;
Layout.NavCard = NavCard;
Layout.Row = Row;
Layout.Column = Column;

export { Layout };
