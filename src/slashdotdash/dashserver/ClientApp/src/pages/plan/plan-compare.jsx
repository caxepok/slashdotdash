import React, { useEffect, useMemo } from "react";
import styled from "styled-components";
import * as Markup from "../shops/shops.styles";
import { DatePicker } from "../../components";
import { setDate } from "../../reducers/dashboard";
import { useDispatch, useSelector } from "react-redux";
import format from "date-fns/format";

export const PlanCompare = () => {
  const date = useSelector(({ dashboard }) => dashboard.date.planCompare);
  const currentDate = useSelector(({ dashboard }) => dashboard.date.plan);
  const dispatch = useDispatch();
  const excludeDates = useMemo(() => [currentDate], [currentDate]);

  useEffect(
    function () {
      if (currentDate && date && format(currentDate, "dd/MM/yyyy") === format(date, "dd/MM/yyyy")) {
        dispatch(setDate(undefined, "planCompare"));
      }
    },
    [dispatch, currentDate, date],
  );

  return (
    <Wrapper>
      (сравнить с
      <Markup.DatePickerWrapper>
        <DatePicker
          excludeDates={excludeDates}
          value={date}
          placeholder="выбрать"
          onChange={(value) => dispatch(setDate(value, "planCompare"))}
        />
      </Markup.DatePickerWrapper>
      )
    </Wrapper>
  );
};

const Wrapper = styled.span`
  margin-left: 0.7em;
  font-weight: 400;
  color: ${(p) => p.theme.colors.secondary};
  white-space: nowrap;
`;
