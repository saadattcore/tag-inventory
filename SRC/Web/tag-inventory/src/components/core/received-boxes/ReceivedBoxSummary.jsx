import React, { Component } from "react";
import Tabs from "@material-ui/core/Tabs";
import Tab from "@material-ui/core/Tab";
import TabPanel from "@material-ui/lab/TabPanel";
import Box from "@material-ui/core/Box";
import TabContext from "@material-ui/lab/TabContext";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import Pagination from "../../common/reuseable/Pagination";
import config from "../../../config.json";
import { Link } from "react-router-dom";
import Table from "../../common/ui-controls/Table";
import Actions from "../../common/reuseable/Actions";
import ImportScanActions from "../../common/reuseable/ImportScanActions";
import ReceivedBoxDetail from "../received-boxes/ReceivedBoxDetail";
import {
  getReceivedBox,
  updateReceivedBox
} from "../../../actions/receivedBoxActions";
import { getTags } from "../../../actions/tagActions";
import { renderAlert } from "../../common/ui-controls/withAlert";

class ReceivedBoxSummary extends Component {
  constructor(props) {
    super(props);
    this.receivedBoxID = this.props.match.params.id
      ? this.props.match.params.id
      : -1;

    this.pageNumber = 1;
    this.filterSetting = { receivedBoxID: this.receivedBoxID };
    this.pageSize = config.pageConfig.pageSize;
  }

  state = {
    receivedBox: {},
    selectedTab: 0,
    isFormValid: false,
    showAlert: null
  };

  columns = [
    {
      path: "tagID",
      title: "Tag ID",
      key: 1,
      content: id => {
        const navigateUrl = `/tag/summary/${id}`;
        return <Link to={navigateUrl}>{id}</Link>;
      }
    },
    { path: "tagNumber", title: "Tag Number", key: 2 },
    { path: "serialNumber", title: "Serial Number", key: 3 },
    { path: "isImported", title: "Is Imported", key: 4 },
    { path: "visualCheckStatus", title: "Visual Check", key: 5 },
    { path: "rfidCheckStatus", title: "RFID Check", key: 6 },
    { path: "status", title: "Tag Status", key: 7 },
    { path: "tagType", title: "Tag Type", key: 8 }
  ];

  componentDidMount() {
    const url = `${process.env.REACT_APP_API_URL}received-box/${this.receivedBoxID}`;
    this.props.getReceivedBoxAction(url).then(response => {
      console.log(response);
      const state = { ...this.state };

      state.receivedBox = { ...response };

      const url = this.getURL();
      console.log(url);
      this.props.getTagsAction(url);

      this.setState(state);
    });
  }

  handleClick(e) {
    e.target.e.preventDefault();

    switch (e.target.action) {
      case "back":
        this.props.history.goBack();
        break;
      case "saveclose":
      case "save":
        const url = `${process.env.REACT_APP_API_URL}received-box`;
        this.props
          .updateReceivedBoxAction(url, { ...this.state.receivedBox })
          .then(r => {
            if (e.target.action === "saveclose") {
              this.props.history.push("/received-box");
            } else {
              const stateClone = { ...this.state };

              stateClone.showAlert = renderAlert.bind(
                null,
                "info",
                "Received box updated sucessfully",
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

  handleChange({ target }) {
    let { name, value } = target;
    const state = { ...this.state };

    switch (name) {
      case "tab":
        state.selectedTab = value;
        state.isFormValid = false;
        break;
      case "statusID":
      case "boxTypeID":
        state.receivedBox[name] = value;
        state.isFormValid = true;

        break;
    }

    this.setState(state);
  }

  handleFilterElementChange({ value, element, column }) {
    switch (element) {
      case "pagination":
        this.pageNumber = parseInt(value);
        break;
      case "dropdown":
        if (parseInt(value) === -1) delete this.filterSetting["statusID"];
        else this.filterSetting["statusID"] = value;
        this.pageNumber = 1;
        break;
      case "tablefilter":
        if (value) this.filterSetting[column] = value;
        else delete this.filterSetting[column];
        this.pageNumber = 1;
        break;
    }

    const url = this.getURL();
    this.props.getTagsAction(url);
  }

  getURL() {
    let baseUrl = `${process.env.REACT_APP_API_URL}tag/list?pageSize=${this.pageSize}&pageNumber=${this.pageNumber}`;
    let props = Object.keys(this.filterSetting);

    if (props.length > 0) {
      for (let prop in this.filterSetting) {
        baseUrl = baseUrl + `&${prop}=${this.filterSetting[prop]}`;
      }
    }
    console.log(baseUrl);
    return baseUrl;
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
            <h3>{this.state.receivedBox.receivedBoxID}</h3>
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
                  <Tab label="Received Tags" />
                </Tabs>
              </Box>
              {this.state.selectedTab === 0 && (
                <TabPanel
                  value={this.state.selectedTab}
                  index={0}
                  sx={{
                    width: "100%",
                    padding: 0
                  }}
                >
                  <div className="content-area-summary top-buffer">
                    <div className="row">
                      <div className="col-md-5">
                        <ReceivedBoxDetail
                          receivedBox={this.state.receivedBox}
                          onChange={this.handleChange.bind(this)}
                          readOnly={true}
                          navigateFrom="detail"
                        ></ReceivedBoxDetail>
                      </div>
                    </div>
                  </div>
                </TabPanel>
              )}
              {this.state.selectedTab === 1 && (
                <TabPanel
                  value={this.state.selectedTab}
                  index={1}
                  sx={{
                    width: "100%",
                    padding: 0
                  }}
                >
                  <div className="content-area-summary top-buffer">
                    <div className="actions-import-box-no-margin">
                      <ImportScanActions
                        actionFormType="scanTags"
                        navigateURL={`/received-box/${this.receivedBoxID}/scan-tags`}
                      ></ImportScanActions>
                    </div>
                    <div className="row top-buffer">
                      <div className="col-3 select-container">
                        <select
                          className="select-search"
                          onChange={e =>
                            this.handleFilterElementChange({
                              value: e.target.value,
                              element: "dropdown"
                            })
                          }
                        >
                          <option className="select-search" value={-1} selected>
                            All Tags
                          </option>
                          <option className="select-search" value={0}>
                            Imported
                          </option>
                          <option className="select-search" value={1}>
                            {" "}
                            Scanned OK
                          </option>
                          <option className="select-search" value={2}>
                            Defective
                          </option>
                          <option className="select-search" value={3}>
                            Discarded
                          </option>
                          <option className="select-search" value={4}>
                            Missing Tag
                          </option>
                          <option className="select-search" value={5}>
                            Missing Kit
                          </option>
                          <option className="select-search" value={6}>
                            Kitted
                          </option>
                        </select>
                      </div>
                    </div>

                    <div className="row top-buffer">
                      <div className="list-container">
                        {
                          <Table
                            columns={this.columns}
                            data={this.props.tags}
                            searchVisible={true}
                            onTableFilter={this.handleFilterElementChange.bind(
                              this
                            )}
                          ></Table>
                        }
                      </div>
                      <div className="row">
                        <Pagination
                          totalRecords={this.props.searchCount}
                          pageSize={this.pageSize}
                          onPageChange={this.handleFilterElementChange.bind(
                            this
                          )}
                          selectedPage={this.pageNumber}
                        ></Pagination>
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

ReceivedBoxSummary.propTypes = {
  receivedBox: PropTypes.object.isRequired,
  updateReceivedBoxAction: PropTypes.func.isRequired,
  modifyStateAction: PropTypes.func.isRequired,
  getReceivedBoxAction: PropTypes.func.isRequired
};

const mapStateToProps = state => {
  return {
    receivedBox: state.receivedBox.receivedBox,
    tags: state.tag.tags,
    totalCount: state.tag.totalCount,
    searchCount: state.tag.searchCount
  };
};

const mapDispatchToProps = dispatch => ({
  getReceivedBoxAction: url => dispatch(getReceivedBox(url)),
  updateReceivedBoxAction: (url, data) =>
    dispatch(updateReceivedBox(url, data)),
  getTagsAction: url => dispatch(getTags(url))
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ReceivedBoxSummary);
