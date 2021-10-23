import * as dataApi from "../services/data";

const initialState = {};

const SET_DATA = "main/SET_DATA";

const mainReducer = (state = initialState, action) => {
  switch (action.type) {
    case SET_DATA:
      return {
        ...state,
        data: action.data,
      };

    default: {
      return state;
    }
  }
};

const setData = (data) => ({
  type: SET_DATA,
  data,
});

export const fetchData = () => async (dispatch) => dispatch(setData(await dataApi.fetchData()));

export default mainReducer;
