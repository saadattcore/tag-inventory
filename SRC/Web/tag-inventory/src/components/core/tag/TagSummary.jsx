import React, { Component } from "react";
import Tabs from "@material-ui/core/Tabs";
import Tab from "@material-ui/core/Tab";
import TabPanel from "@material-ui/lab/TabPanel";
import Box from "@material-ui/core/Box";
import TabContext from "@material-ui/lab/TabContext";
import {
  Timeline,
  TimelineItem,
  TimelineSeparator,
  TimelineDot,
  TimelineConnector,
  TimelineContent,
  TimelineOppositeContent
} from "@material-ui/lab";
import Typography from "@material-ui/core/Typography";
import ChildTimeLine from "../../common/ui-controls/ChildTimeLine";
import { connect } from "react-redux";
import moment from "moment";
import PropTypes from "prop-types";
import { getTag, getTagHistory, UpdateTag } from "../../../actions/tagActions";
import Actions from "../../common/reuseable/Actions";
import { Link } from "react-router-dom";
import TagDetail from "../tag/TagDetail";
import ShipmentDetail from "../shipment/ShipmentDetail";
import ReceivedBoxDetail from "../received-boxes/ReceivedBoxDetail";
import { getLookups } from "../../../actions/lookupActions";
import { renderAlert } from "../../common/ui-controls/withAlert";

import _ from "lodash";

class TagSummary extends Component {
  constructor(props) {
    super(props);
    this.tagID = this.props.match.params.tagID;
    this.counter = 0;
  }
  state = {
    shipmentStatusID: 0,
    selectedTab: 0,
    isFormValid: false,
    tag: {},
    showAlert: null
  };

  componentDidMount() {
    const url = `${process.env.REACT_APP_API_URL}tag/${this.tagID}`;
    this.props.getTagAction(url).then(tag => {
      const url = `${process.env.REACT_APP_API_URL}tag/history/${this.tagID}`;
      this.props.getTagHistoryAction(url).then(r => {
        const state = { ...this.state };
        console.log(tag);
        state.tag = { ...tag };
        this.setState(state);
      });
    });
  }

  handleChange({ target }) {
    let { name, value } = target;
    const state = { ...this.state };

    switch (name) {
      case "tab":
        state.selectedTab = value;
        state.isFormValid = false;
        this.counter = 0;
        break;
      case "pIN":
      case "statusID":
        state.tag[name] = parseInt(value);
        state.isFormValid = true;
        break;
    }

    this.setState(state);
  }

  formatDate(dt) {
    return moment(dt).format("dddd DD MMMM YYYY");
  }

  handleClick(arg) {
    arg.target.e.preventDefault();
    const { action } = arg.target;

    switch (action) {
      case "back":
        this.props.history.goBack();
        break;
      case "save":
      case "saveclose":
        const url = `${process.env.REACT_APP_API_URL}tag/update-tag`;
        this.props.UpdateTagAction(url, { ...this.state.tag }).then(r => {
          if (action === "saveclose") {
            this.props.history.push("/tag");
          } else {
            const stateClone = { ...this.state };

            stateClone.showAlert = renderAlert.bind(
              null,
              "info",
              "Tag have been updated sucessfully.",
              true,
              e => {
                stateClone.showAlert = null;
                this.setState(stateClone);
              }
            );

            this.setState(stateClone);
          }
        });
        break;
    }
  }

  render() {
    return (
      <React.Fragment>
        <div className="actions">
          <Actions
            isFormValid={this.state.isFormValid}
            onLinkClick={this.handleClick.bind(this)}
          ></Actions>
        </div>
        {this.state.showAlert && this.state.showAlert()}
        <div className="title-summary-wrapper top-buffer ">
          <div className="content-title">
            <h3>{this.props.tag.tagID}</h3>
          </div>

          <div className="top-buffer">
            <TabContext value={this.state.selectedTab}>
              <Box
                sx={{ borderBottom: 0, borderColor: "divider", width: "25%" }}
              >
                <Tabs
                  value={this.state.selectedTab}
                  variant="fullWidth"
                  indicatorColor="primary"
                  centered
                  onChange={(e, newValue) =>
                    this.handleChange.call(this, {
                      target: {
                        name: "tab",
                        value: newValue
                      }
                    })
                  }
                  aria-label="basic tabs example"
                >
                  <Tab label="Summary" />
                  <Tab label="History" />
                </Tabs>
              </Box>
              {this.state.selectedTab === 0 && (
                <TabPanel
                  value={this.state.selectedTab}
                  index={0}
                  style={{ padding: 0 }}
                >
                  <div className="content-area-summary top-buffer">
                    <div className="row">
                      <div className="col-4">
                        <TagDetail
                          tag={this.state.tag}
                          readOnly={true}
                          onChange={this.handleChange.bind(this)}
                          tagStatusList={this.props.lookup.tagstatus}
                        ></TagDetail>
                      </div>
                      <div className="col-4">
                        <ReceivedBoxDetail
                          receivedBox={this.props.tag.receivedBox}
                          readOnly={true}
                        ></ReceivedBoxDetail>
                      </div>
                      <div className="col-4">
                        <ShipmentDetail
                          shipment={this.props.tag.receivedBox.shipment}
                          shipmentID={
                            this.props.tag.receivedBox.shipment.shipmentID
                          }
                          shipmentStatusID={
                            this.props.tag.receivedBox.shipment.statusID
                          }
                          errors={{}}
                          readOnly={true}
                          onChangeValue={() => {}}
                        ></ShipmentDetail>
                      </div>
                    </div>
                  </div>
                </TabPanel>
              )}
              {this.state.selectedTab === 1 && (
                <TabPanel
                  value={this.state.selectedTab}
                  index={1}
                  style={{ padding: 0 }}
                >
                  <div className="content-area-summary top-buffer">
                    <div className="row">
                      <div className="col-md-5 time-line-container">
                        <Timeline>
                          {this.props.tagHistory.map(t => {
                            {
                              this.counter++;
                            }
                            return (
                              <React.Fragment>
                                <TimelineItem style={{ color: "#1976d2" }}>
                                  <TimelineSeparator>
                                    <TimelineDot>
                                      {/*    <EventNote /> */}
                                    </TimelineDot>
                                    <TimelineConnector />
                                  </TimelineSeparator>
                                  <TimelineContent>
                                    <Typography variant="h6" component="span">
                                      {this.formatDate(t.dateGroup)}
                                    </Typography>
                                  </TimelineContent>
                                </TimelineItem>
                                <ChildTimeLine
                                  tags={t.tags}
                                  formatDate={this.formatDate.bind(this)}
                                  lastItem={
                                    this.props.tagHistory.length ===
                                    this.counter
                                  }
                                ></ChildTimeLine>
                              </React.Fragment>
                            );
                          })}
                        </Timeline>
                      </div>
                    </div>
                  </div>
                </TabPanel>
              )}
            </TabContext>
          </div>
        </div>
      </React.Fragment>
    );
  }
}

PropTypes.Tag = {
  tag: PropTypes.object.isRequired,
  getTag: PropTypes.func.isRequired
};

const mapStateToProps = state => {
  return {
    tag: state.tag.tag,
    tagHistory: state.tag.tagHistory,
    lookup: state.lookup.lookup
  };
};

const mapDispatchToProps = dispatch => ({
  getTagAction: url => dispatch(getTag(url)),
  getTagHistoryAction: url => dispatch(getTagHistory(url)),
  UpdateTagAction: (url, payload) => dispatch(UpdateTag(url, payload))
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(TagSummary);
