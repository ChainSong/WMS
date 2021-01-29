using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.ForecastWarehouse;
using Runbow.TWS.Dao.ForecastWarehouse;
using Runbow.TWS.Common;

namespace Runbow.TWS.Biz.ForecastWarehouse
{
    public class GapPickingNoteService : BaseService
    {
        public Response<GapPickingNoteResponse> GetGapPickingNote(GapPickingNoteRequest request)
        {
            Response<GapPickingNoteResponse> response = new Response<GapPickingNoteResponse>() { Result = new GapPickingNoteResponse() };
            try
            {
                GapPickingNoteAccessor accessor = new GapPickingNoteAccessor();
                response.Result = accessor.GetGapPickingNote(request);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.Exception = ex;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }


        public Response<GapPickingNoteResponse> AddGapPickingNote(GapPickingNoteRequest request)
        {
            Response<GapPickingNoteResponse> response = new Response<GapPickingNoteResponse>() { Result = new GapPickingNoteResponse() };
            try 
            {
                GapPickingNoteAccessor accessor = new GapPickingNoteAccessor();
                response.SuccessMessage = accessor.AddGapPickingNote(request.GapPickingNote);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.Exception = ex;
                response.ErrorCode = ErrorCode.Technical;
            }
            return response;
        }

    }
}
