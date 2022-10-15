import React from "react";
import { Route, Switch, Link } from "react-router-dom";
import Shipments from "../core/shipment/Shipments";
import USBoxes from "../core/usbox/USBoxes";
import Boxes from "../core/box/Boxes";
import Index from "../core/Index";
import Tags from "../core/tag/Tags";
import Shipment from "../core/shipment/Shipment";

const Content = (props) => {
  const { data } = props;
  return (
    <React.Fragment>
      <div className="row sub-menu">
        <Link to="/shipments/new">Add New Shipment</Link>
      </div>
      <div className="row">
        <div className="dynamic-content">
          <Switch>
            <Route path="/shipments/new" component={Shipment}></Route>
            <Route
              path="/shipments"
              render={() => <Shipments data={data}></Shipments>}
            ></Route>
            <Route path="/usboxes" component={USBoxes}></Route>
            <Route path="/boxes" component={Boxes}></Route>
            <Route path="/tags" component={Tags}></Route>
            <Route path="/" component={Index}></Route>
          </Switch>
        </div>
      </div>
    </React.Fragment>
  );
};

export default Content;
