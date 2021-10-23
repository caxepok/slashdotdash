import React, { useEffect, useMemo } from "react";
import { Title, Wrapper } from "../dashboard/dashboard-item/dashboard-item.styles";
import { ChartBar } from "../../components";
import { shallowEqual, useDispatch, useSelector } from "react-redux";
import { loadShopData } from "../../reducers/dashboard";

export const ShopsItem = React.memo((props) => {
  const { id, date, title } = props;
  const data = useSelector(({ dashboard }) => dashboard.shops[id], shallowEqual);
  const dispatch = useDispatch();
  useEffect(() => dispatch(loadShopData(date, id)), [dispatch, date, id]);

  const convertedData = useMemo(() => {
    return (
      data &&
      data.map((item) => ({
        name: item.resourceGroupId,
        value: item.averageOccupation,
        threshold: item.threshold,
        thresholdDirection: true,
      }))
    );
  }, [data]);

  return (
    <Wrapper>
      <ChartBar values={convertedData} />
      <Title>{title}</Title>
    </Wrapper>
  );
});
