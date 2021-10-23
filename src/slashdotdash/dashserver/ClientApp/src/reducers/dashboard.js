import * as dataApi from "../services/data";

const initialState = {
  date: {
    shop: new Date("2021-10-07T00:00:00"),
    plan: new Date("2021-10-07T00:00:00"),
  },
};

const SET_DATA = "dashboard/SET_DATA";
const SET_DATE = "dashboard/SET_DATE";
const SET_SHOP_DATA = "dashboard/SET_SHOP_DATA";
const SET_PLAN_DATA = "dashboard/SET_PLAN_DATA";
const SET_COMPARE_PLAN_DATA = "dashboard/SET_COMPARE_PLAN_DATA";

const dashboardReducer = (state = initialState, action) => {
  switch (action.type) {
    case SET_DATA:
      return {
        ...state,
        data: action.data,
      };

    case SET_DATE:
      return {
        ...state,
        date: {
          ...state.date,
          [action.area]: action.date,
        },
      };

    case SET_SHOP_DATA: {
      return {
        ...state,
        shopData: action.data,
      };
    }

    case SET_PLAN_DATA: {
      return {
        ...state,
        planData: action.data,
      };
    }

    case SET_COMPARE_PLAN_DATA: {
      return {
        ...state,
        comparePlanData: action.data,
      };
    }

    default: {
      return state;
    }
  }
};

export const loadData = () => async (dispatch) => dispatch({ type: SET_DATA, data: await dataApi.fetchData() });

export const loadShopData = (date) => async (dispatch) =>
  dispatch({ type: SET_SHOP_DATA, data: await dataApi.fetchShopData(date) });
export const loadPlanData = (date) => async (dispatch) =>
  dispatch({ type: SET_PLAN_DATA, data: await dataApi.fetchPlanData(date) });
export const loadComparePlanData = (dateSrc, dateDst) => async (dispatch) =>
  dispatch({ type: SET_COMPARE_PLAN_DATA, data: await dataApi.fetchComparePlanData(dateSrc, dateDst) });
export const setDate = (date, area) => ({ type: SET_DATE, date, area });

export default dashboardReducer;
