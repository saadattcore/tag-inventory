import { GET_DISTRIBUTORS, ADD_LOOKUP } from "../actions/actionTypes";

const stateInitializer = {
  distributors: [],
  distributorTypes: [],
  lookup: { tagstatus: [{}] }
};

const lookupReducer = (state = stateInitializer, action) => {
  switch (action.type) {
    case GET_DISTRIBUTORS:
      return {
        ...state,
        distributors: [...action.payload.distributors],
        distributorTypes: [...action.payload.distributorTypes]
      };
    case ADD_LOOKUP:
      return {
        ...state,
        lookup: { ...action.payload }
      };
    default:
      return state;
  }
};

export default lookupReducer;
