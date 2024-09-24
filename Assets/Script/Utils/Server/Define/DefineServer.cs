public enum ProviderType
{
    Guest,
    GooglePlayGames,
    GameCenter,
    Facebook,
    Google,
    Apple,
}
public enum HttpHeaderKey
{
    AccountId,
    JwtToken,
}
public enum HttpResponceMessageType
{
    Unknown = 0,
    Continue = 100,
    OK = 200,
    BadRequest = 400,

    // ** 1000 ~ costom
    NotFoundResponseType = 1000, // 기존 에러 코드에서 대칭되는 코드를 못찾음 

    // ** 1400 부터 에러 메세지 
    // 공통 관련 에러 
    NotFoundConfiguration = 1400, // Configuration 정보가 잘못됨
    NotFoundHeader = 1401, // Header 정보가 잘못됨
    IpFilter = 1402,
    NotSetRadisTableData, // 레디스에 테이블 데이터 셋팅 전
    NotFoundTableData, // 테이블에서 찾을수 없는 데이터 전송

    // 로그인 관련 에러 : 1500 ~ 1599  
    InvalidJwtToken = 1500, // 잘못된 유형의 Jwt Token
    ExpiredJwtToken = 1501, // 기한 만료된 Jwt Token
    FailAccountAdd = 1502, // 계정 만들기 실패
    FailCacheAddJwtToken = 1503, // 레디스에 jwt 정보 넣기 실패
    DuplicationAccount, // 중복 로그인
    NotAdmin, // 어드민 계정이 아닙니다
    AutoLoginExpire, // 자동 로그인 만료

    // 플레이어 데이터 - 아이템
    FailedPushPlayerDataItem = 1600,
}
public enum SseEvent
{
    Announce,
}
public enum ChatChannel
{
    All
}