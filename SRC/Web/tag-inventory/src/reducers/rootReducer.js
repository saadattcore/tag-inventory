import { combineReducers } from "redux";
import shipmentReducer from "./shipmentReducer";
import receivedBoxReducer from "./receivedBoxReducer";
import tagReducer from "./tagReducer";
import issuedBoxReducer from "./issuedBoxReducer";
import lookupReducer from "./lookupReducer";

const rootReducer = combineReducers({
  shipment: shipmentReducer,
  receivedBox: receivedBoxReducer,
  tag: tagReducer,
  issuedBox: issuedBoxReducer,
  lookup: lookupReducer
});

export default rootReducer;
