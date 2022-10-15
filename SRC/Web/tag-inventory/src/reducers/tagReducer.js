import {
  GET_TAGS,
  RESET_TAGS,
  GET_TAG,
  UPDATE_SCAN_TAGS,
  GET_TAG_HISTORY,
  UPDATE_TAG,
} from "../actions/actionTypes";

const stateInitilaizer = {
  tags: [],
  tag: { receivedBox: { shipment: {} } },
  boxScannedTags: {
    receivedBox: { receivedBoxID: -1, status: "", updUserID: -1, tags: [] },
  },
  totalCount: 0,
  searchCount: 0,
  tagHistory: [],
};

const tagReducer = (state = stateInitilaizer, action) => {
  switch (action.type) {
    case GET_TAGS:
      return {
        ...state,
        tags: action.payload.tags,
        totalCount: action.payload.totalCount,
        searchCount: action.payload.searchCount,
      };
    case RESET_TAGS:
      return {
        ...state,
        tags: [],
        totalCount: 0,
        searchCount: 0,
      };

    case GET_TAG:
    case UPDATE_TAG:
      return {
        ...state,
        tag: action.payload.tag,
      };

    case GET_TAG_HISTORY:
      return {
        ...state,
        tagHistory: [...action.payload],
      };

    default:
      return state;
  }
};

export default tagReducer;
