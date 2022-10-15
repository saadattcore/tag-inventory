import {
  GET_ISSUED_BOXES,
  RESET_ISSUED_BOXES,
  CREATE_ISSUED_BOX,
  UPDATE_ISSUED_BOX_TAGS,
  GET_ISSUED_BOX,
  APPEND_SCANTAG_TO_ISSUEDBOX,
  APPEND_SCANTAG_TO_EXISTING_BOX,
  SELECT_ISSUED_BOX,
  GET_ISSUED_BOX_HISTORY,
  UPDATE_ISSUED_BOXES_STATUS,
  UPDATE_ISSUED_BOX
} from "../actions/actionTypes";

const stateInitializer = {
  issuedBoxes: [],
  createdIssuedBox: {},
  issuedBox: {},
  searchCount: 0,
  totalCount: 0,
  scanTags: [],
  issuedBoxHistory: []
};

const issuedBoxReducer = (state = stateInitializer, action) => {
  switch (action.type) {
    case GET_ISSUED_BOXES:
      return {
        ...state,
        issuedBoxes: action.payload.issuedBoxes,
        searchCount: action.payload.searchCount,
        totalCount: action.payload.totalCount
      };

    case CREATE_ISSUED_BOX:
    case UPDATE_ISSUED_BOX_TAGS:
      return { ...state, issuedBox: action.payload.issuedBox };

    case RESET_ISSUED_BOXES:
      return {
        ...state,
        issuedBoxes: [],
        searchCount: 0,
        totalCount: 0
      };

    case GET_ISSUED_BOX:
      return {
        ...state,
        issuedBox: action.payload.issuedBox
      };

    case APPEND_SCANTAG_TO_ISSUEDBOX:
      /*    const issuedBoxNew = { ...state.createdIssuedBox };
      issuedBoxNew.tags.push({ ...action.payload.scanTag });
      issuedBoxNew.quantity = issuedBoxNew.tags.length;
      return {
        ...state,
        createdIssuedBox: issuedBoxNew
      }; */

      let tmpScanTags = [];
      tmpScanTags.push({ ...action.payload.scanTag });

      return {
        ...state,
        scanTags: [...tmpScanTags]
      };

    case APPEND_SCANTAG_TO_EXISTING_BOX:
      const cloneExistingBox = { ...state.issuedBox };
      cloneExistingBox.tags.push({ ...action.payload.scanTag });
      cloneExistingBox.quantity = cloneExistingBox.tags.length;
      return {
        ...state,
        issuedBox: cloneExistingBox
      };

    case SELECT_ISSUED_BOX:
      const issuedBoxList = [...state.issuedBoxes];

      console.log(issuedBoxList);
      console.log(action);

      if (action.payload.all) {
        issuedBoxList.forEach(b => (b.selected = action.payload.checked));
      } else {
        const box = issuedBoxList.filter(
          b => b.issuedBoxID === action.payload.box.issuedBoxID
        );

        box.selected = action.payload.box.selected;
      }

      return { ...state, issuedBoxes: issuedBoxList };

    case GET_ISSUED_BOX_HISTORY:
      return {
        ...state,
        issuedBoxHistory: [...action.payload]
      };
    case UPDATE_ISSUED_BOXES_STATUS:
      if (action.payload.length === 1)
        return { ...state, issuedBox: action.payload };
      else return { ...state, issuedBoxes: action.payload };

    case UPDATE_ISSUED_BOX:
      return { ...state, issuedBox: action.payload };
      break;

    default:
      return state;
  }
};

export default issuedBoxReducer;
