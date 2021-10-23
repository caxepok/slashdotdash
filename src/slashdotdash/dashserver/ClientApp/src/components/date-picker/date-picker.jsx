import React, { useCallback } from "react";
import ReactDatePicker from "react-datepicker";
import { ru } from "date-fns/locale";
import * as Markup from "./date-picker.styles";
import "react-datepicker/dist/react-datepicker.min.css";
import { useDispatch, useSelector } from "react-redux";
import { setDate } from "../../reducers/dashboard";

const CustomInput = React.forwardRef(({ value, onClick }, ref) => {
  return (
    <Markup.Wrapper onClick={onClick} ref={ref}>
      {value}
    </Markup.Wrapper>
  );
});

export const DatePicker = React.memo(() => {
  const dispatch = useDispatch();
  const date = useSelector(({ dashboard }) => dashboard.date);
  const handleChange = useCallback((date) => dispatch(setDate(date)), [dispatch]);

  return (
    <ReactDatePicker
      locale={ru}
      dateFormat="dd.MM.yyyy"
      disabledKeyboardNavigation
      selected={date}
      onChange={handleChange}
      customInput={<CustomInput />}
    />
  );
});
