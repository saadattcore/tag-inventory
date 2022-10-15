import React, { useState, useEffect, useRef } from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import { getTags } from "../../../actions/tagActions";
import config from "../../../config.json";
import Table from "../../common/ui-controls/Table";
import Actions from "../../common/reuseable/Actions";
import ImportScanActions from "../../common/reuseable/ImportScanActions";
import Pagination from "../../common/reuseable/Pagination";
import { Link } from "react-router-dom";

const Tags = props => {
  const pageNumber = useRef(1);
  const filterSetting = useRef({});
  const pageSize = config.pageConfig.pageSize;
  const receivedBoxID = props.match.params.boxid;
  if (receivedBoxID) filterSetting.current["receivedBoxID"] = receivedBoxID;

  const columns = [
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
    { path: "tagType", title: "Tag Type", key: 7 }
  ];

  const getURL = () => {
    let baseUrl = `${process.env.REACT_APP_API_URL}tag/list?pageSize=${pageSize}&pageNumber=${pageNumber.current}`;
    let props = Object.keys(filterSetting.current);

    if (props.length > 0) {
      for (let prop in filterSetting.current) {
        baseUrl = baseUrl + `&${prop}=${filterSetting.current[prop]}`;
      }
    }
    console.log(baseUrl);
    return baseUrl;
  };

  useEffect(() => {
    const url = getURL();
    console.log(url);
    props.getTagsAction(url);
  }, []);

  const handleChange = async ({ value, element, column }) => {
    switch (element) {
      case "pagination":
        pageNumber.current = parseInt(value);
        break;
      case "dropdown":
        if (parseInt(value) === -1) delete filterSetting.current["statusID"];
        else filterSetting.current["statusID"] = value;
        pageNumber.current = 1;
        break;
      case "tablefilter":
        if (value) filterSetting.current[column] = value;
        else delete filterSetting.current[column];
        pageNumber.current = 1;
        break;
    }

    const url = getURL();
    props.getTagsAction(url);
  };

  const handleClick = arg => {
    switch (arg.target.action) {
      case "back":
        props.history.goBack();
        break;
    }
  };

  return (
    <React.Fragment>
      <div className="actions">
        {props.navigateFrom === "receivedBoxTags" ? (
          <Actions onLinkClick={handleClick}></Actions>
        ) : (
          <ImportScanActions
            actionFormType="scanTags"
            navigateURL={
              receivedBoxID
                ? `/received-box/${receivedBoxID}/scan-tags`
                : "/tag/scan"
            }
          ></ImportScanActions>
        )}
      </div>
      <div className="content-area-main">
        {props.navigateFrom === "receivedBoxTags" && (
          <div>
            <div className="content-title">
              <h3>{receivedBoxID}</h3>
            </div>
            <div className="shipment-detail-summary">
              <Link to={`/received-box/${receivedBoxID}`}>Summary</Link> &nbsp;
              | &nbsp;
              <span>Received Tags</span>
            </div>
            <div className="actions-import-box">
              <ImportScanActions
                actionFormType="scanTags"
                navigateURL={
                  receivedBoxID
                    ? `/received-box/${receivedBoxID}/scan-tags`
                    : "/tag/scan"
                }
              ></ImportScanActions>
            </div>
          </div>
        )}
        <div className="row top-buffer">
          <div className="col-3 select-container">
            <select
              className="select-search"
              onChange={e =>
                handleChange({
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
                columns={columns}
                data={props.tags}
                searchVisible={true}
                onTableFilter={handleChange}
              ></Table>
            }
          </div>
          <div className="row">
            <Pagination
              totalRecords={props.searchCount}
              pageSize={pageSize}
              onPageChange={handleChange}
              selectedPage={pageNumber.current}
            ></Pagination>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

PropTypes.Tags = {
  tags: PropTypes.array.isRequired,
  totalCount: PropTypes.number.isRequired,
  searchCount: PropTypes.number.isRequired
};

const mapStateToProps = state => {
  return {
    tags: state.tag.tags,
    totalCount: state.tag.totalCount,
    searchCount: state.tag.searchCount
  };
};

const mapDispatchToProps = dispatch => ({
  getTagsAction: url => dispatch(getTags(url))
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Tags);
