import React, { useCallback, useEffect, useRef, useState } from "react";
import useEventListener from "@use-it/event-listener";
import { ReactComponent as PinIcon } from "../../assets/pin.svg";
import * as Markup from "./popup.styles";
const OFFSET = 20;

export const Popup = React.memo((props) => {
  const { triggerRef, children, title } = props;
  const [isVisible, setVisibility] = useState();
  const [isFixed, setFixed] = useState();
  const [dragParams, setDragParams] = useState();
  const [style, setStyle] = useState();
  useEffect(() => setVisibility(false), []);
  const blockRef = useRef(null);

  const handleClick = useCallback(
    (e) => {
      if (blockRef.current && blockRef.current.contains(e.target)) {
        e.preventDefault();
        return false;
      }
      setFixed((value) => !value);
    },
    [blockRef],
  );
  useEventListener("click", handleClick, triggerRef.current);

  const handleMouseOver = useCallback(
    (e) => {
      setVisibility(
        (triggerRef.current && triggerRef.current.contains(e.target)) ||
          (blockRef.current && blockRef.current.contains(e.target)),
      );
    },
    [triggerRef, blockRef],
  );
  useEventListener("mousemove", handleMouseOver, !isFixed ? document : null);

  const handleMouseDown = useCallback(
    (e) => {
      if (!style) return;
      if (e.which !== 1) return;

      setDragParams({
        left: style.left,
        top: style.top,
        x: e.pageX,
        y: e.pageY,
      });
    },
    [style],
  );
  const handleMouseMove = useCallback(
    (e) => {
      if (!dragParams) {
        return;
      }
      setStyle({
        left: dragParams.left + e.pageX - dragParams.x,
        top: dragParams.top + e.pageY - dragParams.y,
        opacity: 1,
      });
    },
    [dragParams],
  );
  const handleMouseUp = useCallback(() => setDragParams(undefined), []);
  useEventListener("mousedown", handleMouseDown, isFixed ? blockRef.current : null);
  useEventListener("mousemove", handleMouseMove, dragParams ? document.body : null);
  useEventListener("mouseup", handleMouseUp, dragParams ? document.body : null);

  useEffect(() => {
    if (isVisible && blockRef.current && triggerRef.current) {
      const blockRect = blockRef.current.getBoundingClientRect();
      const triggerRect = triggerRef.current.getBoundingClientRect();
      const alignTop = triggerRect.top + triggerRect.height / 2 + blockRect.height + OFFSET > window.innerHeight;
      setStyle({
        top: alignTop
          ? triggerRect.top + triggerRect.height / 2 - blockRect.height - OFFSET
          : triggerRect.top + triggerRect.height / 2 + OFFSET,
        left: Math.min(
          window.innerWidth - OFFSET - blockRect.width,
          Math.max(OFFSET, triggerRect.left + triggerRect.width / 2 - blockRect.width / 2),
        ),
        opacity: 1,
      });
    } else {
      !isFixed && setStyle(undefined);
    }
  }, [isVisible, isFixed, blockRef, triggerRef]);

  useEffect(() => {
    if (isFixed === false) {
      setVisibility(false);
    }
  }, [isFixed]);

  return isVisible || isFixed ? (
    <Markup.Wrapper ref={blockRef} style={style} isFixed={isFixed} tabIndex={0}>
      <Markup.Header>
        {title}
        <Markup.Button isFixed={isFixed}>
          <PinIcon onClick={() => setFixed((value) => !value)} />
        </Markup.Button>
      </Markup.Header>
      {children}
    </Markup.Wrapper>
  ) : null;
});
