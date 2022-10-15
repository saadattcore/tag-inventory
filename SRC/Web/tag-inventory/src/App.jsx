import React, { Component } from "react";
import Header from "./components/layout/Header";
import LeftPanel from "./components/layout/LeftPanel";
import Footer from "./components/layout/Footer";
import http from "./services/HttpModule";
import { Route, Switch, Link } from "react-router-dom";
import Shipments from "./components/core/shipment/Shipments";
import ReceivedBoxes from "./components/core/received-boxes/ReceivedBoxes";
import ReceivedBox from "./components/core/received-boxes/ReceivedBox";
import ReceivedBoxSummary from "./components/core/received-boxes/ReceivedBoxSummary";

import IssuedBoxes from "./components/core/issued-boxes/IssuedBoxes";
import IssuedBox from "./components/core/issued-boxes/IssuedBox";
import IssuedBoxItem from "./components/core/issued-boxes/IssuedBoxItem";
import IssuedBoxTags from "./components/core/issued-boxes/IssuedBoxTags";
import Index from "./components/core/Index";
import Tags from "./components/core/tag/Tags";
import Tag from "./components/core/tag/Tag";
import TagSummary from "./components/core/tag/TagSummary";
import TagHistory from "./components/core/tag/TagHistory";
import Shipment from "./components/core/shipment/Shipment";
import ShipmentSummary from "./components/core/shipment/ShipmentSummary";

import ImportBox from "./components/core/received-boxes/ImportBox";
import ScanTags from "./components/core/tag/ScanTags";
import VerifyKittedTags from "./components/core/issued-boxes/VerifyKittedTags";
import IssuedBoxSendToPress from "./components/core/issued-boxes/IssuedBoxSendToPress";
import IssueBoxToDistributor from "./components/core/issued-boxes/IssueBoxToDistributor";
import IssuedBoxSummary from "./components/core/issued-boxes/IssuedBoxSummary";

import config from "./config.json";
import { getLookups } from "./actions/lookupActions";
import { connect } from "react-redux";

import "./App.css";
import IssuedBoxTimeLine from "./components/core/issued-boxes/IssuedBoxTimeLine";

class App extends Component {
  pageConfig = { pageSize: 10, pageNumber: 1 };
  state = {
    currlink: "Home",
    shipment: [],
    itemsCount: 0,
    lookup: {}
  };

  constructor() {
    super();
  }

  componentDidMount() {
    const stateCopy = { ...this.state };

    const url = process.env.REACT_APP_API_URL + "lookup?key=all";
    this.props.getLookupsAction(url).then(r => {});
    console.log("component Did Mount");

    //sessionStorage.
  }

  handleMenuCick(arg) {
    const cloneState = { ...this.state };
    cloneState.currlink = arg;
    this.setState(cloneState);
    console.log(arg);
  }

  getData_(menu) {
    const { data, itemsCount } = this.httpClient.getData(
      "repository.js",
      this.pageConfig
    );

    console.log(data);
    console.log(itemsCount);

    menu
      ? this.setState({ currlink: menu, data: data, itemsCount: itemsCount })
      : this.setState({ data: data, itemsCount: itemsCount });
  }

  render() {
    return (
      <div className="fluid-container main-container">
        <div className="row nav-container" id="header">
          <Header link={this.state.currlink}></Header>
        </div>
        <div className="row" id="content">
          <div className="col-2 left-panel">
            <LeftPanel
              onMenuClick={this.handleMenuCick.bind(this)}
              selectedMenu={this.state.currlink}
            ></LeftPanel>
          </div>
          <div className="col-10 right-panel">
            <div className="row">
              <div className="dynamic-content">
                <Switch>
                  <Route path="/shipment/create" component={Shipment}></Route>
                  <Route
                    path="/shipment/received-boxes/:shipmentId"
                    component={ReceivedBoxes}
                  ></Route>
                  <Route
                    path="/shipment/summary/:id"
                    component={ShipmentSummary}
                  ></Route>
                  <Route path="/shipment/:id" component={Shipment}></Route>
                  <Route path="/shipment" component={Shipments}></Route>
                  <Route
                    path="/received-box/tags/:boxid"
                    render={props => (
                      <Tags {...props} navigateFrom="receivedBoxTags"></Tags>
                    )}
                  ></Route>
                  <Route
                    path="/received-box/import"
                    component={ImportBox}
                  ></Route>
                  <Route
                    path="/received-box/:receivedBoxID/scan-tags"
                    component={ScanTags}
                  ></Route>

                  <Route
                    path="/received-box/summary/:id"
                    component={ReceivedBoxSummary}
                  ></Route>

                  <Route
                    path="/received-box/:id"
                    component={ReceivedBox}
                  ></Route>
                  <Route
                    path="/received-box"
                    render={props => (
                      <ReceivedBoxes
                        {...props}
                        navigateFrom="leftMenu"
                        lookups={this.state.lookup}
                      ></ReceivedBoxes>
                    )}
                  ></Route>

                  <Route
                    path="/issued-box/summary/:issuedBox"
                    component={IssuedBoxSummary}
                  ></Route>

                  <Route
                    path="/issued-box/history/:issuedBox"
                    component={IssuedBoxTimeLine}
                  ></Route>
                  <Route
                    path="/issued-box/send-to-press"
                    component={IssuedBoxSendToPress}
                  ></Route>

                  <Route
                    path="/issued-box/issue-box-to-distributor"
                    component={IssueBoxToDistributor}
                  ></Route>

                  <Route
                    path="/issued-box/tags/:issuedBoxID"
                    component={IssuedBoxTags}
                  ></Route>
                  <Route
                    path="/issued-box/verify-kitted-tags"
                    component={VerifyKittedTags}
                  ></Route>
                  <Route
                    path="/issued-box/create/:issuedBoxID"
                    component={IssuedBox}
                  ></Route>
                  <Route
                    path="/issued-box/create"
                    component={IssuedBox}
                  ></Route>
                  <Route
                    path="/issued-box/:id"
                    component={IssuedBoxItem}
                  ></Route>
                  <Route path="/issued-box" component={IssuedBoxes}></Route>
                  <Route
                    path="/tag/scan"
                    render={props => (
                      <ScanTags {...props} navigateFrom={"leftMenu"}></ScanTags>
                    )}
                  ></Route>

                  <Route
                    path="/tag/summary/:tagID"
                    component={TagSummary}
                  ></Route>

                  <Route
                    path="/tag/history/:tagID"
                    component={TagHistory}
                  ></Route>
                  <Route path="/tag/:tagID" component={Tag}></Route>
                  <Route path="/tag" component={Tags}></Route>
                  <Route path="/" component={Index}></Route>
                </Switch>
              </div>
            </div>
          </div>
        </div>
        {/*   <div className="row">
          <Footer></Footer>
        </div> */}
      </div>
    );
  }
}

const mapDispatchToProps = dispatch => ({
  getLookupsAction: url => dispatch(getLookups(url))
});

export default connect(
  null,
  mapDispatchToProps
)(App);
