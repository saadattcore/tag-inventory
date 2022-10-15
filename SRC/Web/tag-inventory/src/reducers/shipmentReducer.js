import {
  GET_SHIPMENTS,
  GET_SHIPMENT,
  RESET_SHIPMENTS,
  ADD_UPDATE_SHIPMENT
} from "../actions/actionTypes";

const stateInitializer = {
  shipments: [],
  shipment: {},
  totalCount: 0,
  searchCount: 0
};

const shipmentReducer = (state = stateInitializer, action) => {
  switch (action.type) {
    case GET_SHIPMENTS:
      return {
        ...state,
        shipments: action.payload.shipments,
        totalCount: action.payload.totalCount,
        searchCount: action.payload.searchCount
      };
    case GET_SHIPMENT:
      return {
        ...state,
        shipment: action.payload.shipment
      };

    case RESET_SHIPMENTS:
      return {
        ...state,
        shipments: [],
        totalCount: 0,
        searchCount: 0
      };
    case ADD_UPDATE_SHIPMENT:
      return {
        ...state,
        shipment: action.payload.shipment
      };

    default:
      return state;
  }
};

export default shipmentReducer;
