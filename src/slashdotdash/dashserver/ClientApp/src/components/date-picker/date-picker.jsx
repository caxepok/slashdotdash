import React, { useState } from "react";
import ReactDatePicker from "react-datepicker";
import { ru } from "date-fns/locale";
import * as Markup from "./date-picker.styles";
import "react-datepicker/dist/react-datepicker.min.css";

const CustomInput = React.forwardRef(({ value, onClick }, ref) => {
  return (
    <Markup.Wrapper onClick={onClick} ref={ref}>
      {value}
    </Markup.Wrapper>
  );
});

export const DatePicker = React.memo(() => {
  const [date, setDate] = useState(new Date());
  return (
    <ReactDatePicker
      locale={ru}
      dateFormat="dd.MM.yyyy"
      disabledKeyboardNavigation
      selected={date}
      onChange={setDate}
      customInput={<CustomInput />}
    />
  );
});
