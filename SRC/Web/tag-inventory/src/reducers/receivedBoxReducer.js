import {
  GET_RECEIVED_BOXES,
  GET_RECEIVED_BOX,
  RESET_RECEIVED_BOXES_STATE,
  UPDATE_RECEIVED_BOX,
  UPDATE_SCAN_TAGS,
  ISSUED_BOX_SCAN_TAG,
} from "../actions/actionTypes";

const stateInitializer = {
  receivedBoxes: [],
  receivedBox: {},
  shipment: {},
  totalCount: 0,
  searchCount: 0,
};

const receivedBoxReducer = (state = stateInitializer, action) => {
  switch (action.type) {
    case GET_RECEIVED_BOXES:
      return {
        ...state,
        receivedBoxes: action.payload.receivedBoxes,
        shipment: { ...action.payload.receivedBoxes[0].shipment },
        totalCount: action.payload.totalCount,
        searchCount: action.payload.searchCount,
      };

    case RESET_RECEIVED_BOXES_STATE:
      return {
        ...state,
        receivedBoxes: [],
        shipment: {},
        totalCount: 0,
        searchCount: 0,
      };
    case GET_RECEIVED_BOX:
    case UPDATE_RECEIVED_BOX:
      return {
        ...state,
        receivedBox: action.payload.receivedBox,
      };
    case UPDATE_SCAN_TAGS:
      return {
        ...state,
        receivedBox: action.payload.receivedBox,
      };

    case ISSUED_BOX_SCAN_TAG:
      const stateClone = { ...state };
      console.log(stateClone);
      const tags = [...stateClone.receivedBox.boxTags];
      tags.push(action.payload.scanTag);
      const rb = { ...state.receivedBox };
      rb.boxTags = [...tags];
      console.log(tags);

      return {
        ...state,
        receivedBox: rb,
      };

    default:
      return state;
  }
};

export default receivedBoxReducer;
